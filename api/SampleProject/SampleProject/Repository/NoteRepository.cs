using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleProject.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SampleProject.Repository
{
    public class NoteRepository
    {
        public string CreateNote(Note newNote)
        {
            try
            {
                DateTime today = DateTime.Now;

                string attributeIdString = null;
                if(newNote.AttributeIds!= null && newNote.AttributeIds.Count != 0)
                {
                    attributeIdString = string.Join<int>(",", newNote.AttributeIds);
                }
                string query;

                if(attributeIdString != null && newNote.ProjectId != null)
                {
                    query = @"
                insert into dbo.Notes(CreationTimestamp, NoteText, ProjectId, AttributeIds) values 
                ('" + today + "','" + newNote.NoteText + "'," + newNote.ProjectId + ",'" + attributeIdString + "'" + @")
            ";
                }
                else if (newNote.ProjectId != null && attributeIdString == null )
                {
                    query = @"
                insert into dbo.Notes(CreationTimestamp, NoteText, ProjectId) values 
                ('" + today + "','" + newNote.NoteText + "'," + newNote.ProjectId + @")
            ";
                }
                else if (attributeIdString != null & newNote.ProjectId == null)
                {
                    query = @"
                insert into dbo.Notes(CreationTimestamp, NoteText, AttributeIds) values 
                ('" + today + "','" + newNote.NoteText + "','" + attributeIdString + "'" + @")
            ";
                }
                else
                {
                    query = @"
                insert into dbo.Notes(CreationTimestamp, NoteText) values 
                ('" + today + "','" + newNote.NoteText + "'" + @")
            ";
                }
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Success";

            }
            catch (Exception e)
            {
                return "Fail";
            }
        }
        public List<ProjectNoteCounts> GetProjectNoteCounts()
        {
            var notes = GetAllNotes();
            var projects = GetAllProjects();
            var projectCountListFromTable = projects.Select(c => new ProjectNoteCounts { ProjectId = c.ProjectId, ProjectName = c.ProjectName, Count = 0 }).ToList();
            var emptyProjectIdCount = notes.Where(x => x.ProjectId == null).Count();
            var projectCountList = new List<ProjectNoteCounts> { new ProjectNoteCounts { ProjectId = null, ProjectName = "No Attributes", Count = emptyProjectIdCount } };
            var notesWithProjectIds = notes.Where(x => x.ProjectId != null).ToList();
            foreach (Note note in notesWithProjectIds)
            {
                int index = projectCountListFromTable.FindIndex(m => m.ProjectId == note.ProjectId);
                if (index >= 0)
                    projectCountListFromTable[index].Count++;            
            }
            return projectCountList.Concat(projectCountListFromTable).ToList();
        }
        public List<AttributeNoteCounts> GetAttributeNoteCounts()
        {
            var notes = GetAllNotes();
            var attributes = GetAllAttributes();
            var attributeCountListFromTable = attributes.Select(c => new AttributeNoteCounts { AttributeId = c.AttributeId, AttributeName = c.AttributeName, Count = 0 }).ToList();
            var emptyAttributeIdCount = notes.Where(x => x.AttributeIds == null || x.AttributeIds.Count == 0).Count();
            var attributeCountList = new List<AttributeNoteCounts> { new AttributeNoteCounts { AttributeId = null, AttributeName = "No Attributes", Count = emptyAttributeIdCount } };
            var notesWithAttributeIds = notes.Where(x => x.AttributeIds != null && x.AttributeIds.Count != 0).ToList();
            foreach (Note note in notesWithAttributeIds)
            {
                var attData = note.AttributeIds;
                foreach(int attribute in attData)
                {
                    int index = attributeCountListFromTable.FindIndex(m => m.AttributeId == attribute);
                    if (index >= 0)
                        attributeCountListFromTable[index].Count++;
                }
            }
            return attributeCountList.Concat(attributeCountListFromTable).ToList();
        }
        public string CreateProject(Project newProject)
        {
            try
            {
                string query = @"
                insert into dbo.Project values 
                ('" + newProject.ProjectName + @"')
            ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Success";

            }
            catch (Exception e)
            {
                return "fail";
            }
        }
        public string DeleteNote(int noteId)
        {
            try
            {
                string query = @"
                delete from dbo.Notes where NoteId = " + noteId;

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Success";

            }
            catch (Exception e)
            {
                return "Fail";
            }
        }
        public List<Note> GetNotesWithFilters(int? projectId, List<int> attributeIds)
        {
            try {
                if (projectId != null && attributeIds != null)
                {
                    List<Note> noteList = GetAllNotes();
                    noteList = GetNotesBasedOnAttributes(noteList, attributeIds);
                    return noteList.Where(x => x.ProjectId == projectId).ToList();
                }
                else if (projectId == null && attributeIds != null)
                {
                    List<Note> noteList = GetAllNotes();
                    return GetNotesBasedOnAttributes(noteList, attributeIds);
                }
                else
                {
                    List<Note> noteList = GetAllNotes();
                    return noteList.Where(x => x.ProjectId == projectId).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Note> GetAllNotes()
        {
            try
            {
                string query = "select * from dbo.notes";

                DataSet ds = new DataSet();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(ds);
                    con.Close();
                }

                var noteList = ds.Tables[0].AsEnumerable()
                        .Select(dataRow => new Note
                        {
                            NoteId = dataRow.Field<int>("NoteId"),
                            NoteText = dataRow.Field<string>("NoteText"),
                            CreationTimestamp = dataRow.Field<DateTime>("CreationTimestamp"),
                            ProjectId = dataRow.Field<int?>("ProjectId"),
                            AttributeIds = GetAttributeIdList(dataRow.Field<string>("AttributeIds"))
                        }).ToList();

                return noteList;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Note> GetNotesBasedOnAttributes(List<Note> noteList, List<int> attributeDataList)
        {
            noteList = noteList.Where(x => x.AttributeIds != null).ToList();
            noteList = noteList.Where(x => x.AttributeIds.Any(item => attributeDataList.Contains(item))).ToList();
            return noteList;
        }
        public List<int> GetAttributeIdList(string attributeIdString)
        {
            if (attributeIdString == null)
                return null;

            string[] ids = attributeIdString.Split(',');
            return ids.Select(int.Parse).ToList();
        }
        public List<Project> GetAllProjects()
        {
            try
            {
                string query = "select * from dbo.project";

                DataSet ds = new DataSet();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(ds);
                    con.Close();
                }

                var projectList = ds.Tables[0].AsEnumerable()
                        .Select(dataRow => new Models.Project
                        {
                            ProjectId = dataRow.Field<int>("ProjectId"),
                            ProjectName = dataRow.Field<string>("ProjectName"),
                        }).ToList();

                return projectList;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Models.Attribute> GetAllAttributes()
        {
            try
            {
                string query = "select * from dbo.attribute";

                DataSet ds = new DataSet();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(ds);
                    con.Close();
                }

                var noteList = ds.Tables[0].AsEnumerable()
                        .Select(dataRow => new Models.Attribute
                        {
                            AttributeId = dataRow.Field<int>("AttributeId"),
                            AttributeName = dataRow.Field<string>("AttributeName"),
                        }).ToList();

                return noteList;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}