using SampleProject.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using SampleProject.Repository;

namespace SampleProject.Controllers
{
    public class NoteController : ApiController
    {

        //UpdateNote(token, NoteId, NoteText)
        //http://localhost:60977/api/note/update-note?noteId=1&noteText=hello
        [HttpGet]
        [Route("api/note/update-note")]
        public string UpdateNote(int noteId, string noteText)
        {
            try
            {
                string query = @"
                update dbo.Notes set NoteText = '" + noteText + "' where NoteId = " + noteId;

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

        [HttpPost]
        [Route("api/note/create-note")]
        public string NewNote(Note newNote)
        {
            NoteRepository noteRepository = new NoteRepository();
            return noteRepository.CreateNote(newNote);
        }

        //http://localhost:60977/api/note/delete-note?noteId=1
        [HttpPost]
        [Route("api/note/delete-note")]
        public string DeleteNote(int noteId)
        {
            NoteRepository noteRepository = new NoteRepository();
            return noteRepository.DeleteNote(noteId);
        }

        [HttpPost]
        [Route("api/note/create-project")]
        public string NewProject(Project project)
        {
            NoteRepository noteRepository = new NoteRepository();
            return noteRepository.CreateProject(project);
        }

        [HttpPost]
        [Route("api/note/get-notes")]
        public List<Note> GetNotes([FromBody]List<int>attributeIds, int? projectId = null)
        {
            NoteRepository noteRepository = new NoteRepository();
            if (projectId == null && (attributeIds == null || attributeIds.Count == 0) )
            {
                return noteRepository.GetAllNotes();
            }

            return noteRepository.GetNotesWithFilters(projectId, attributeIds);
        }

        [HttpGet]
        [Route("api/note/get-project-note-counts")]
        public List<ProjectNoteCounts> GetProjectNoteCounts()
        {
            NoteRepository noteRepository = new NoteRepository();
            return noteRepository.GetProjectNoteCounts();
        }

        [HttpGet]
        [Route("api/note/get-attribute-note-counts")]
        public List<AttributeNoteCounts> GetAttributeNoteCounts()
        {
            NoteRepository noteRepository = new NoteRepository();
            return noteRepository.GetAttributeNoteCounts();
        }

        [HttpGet]
        [Route("api/note/get-lists")]
        public Lists GetLists()
        {
            NoteRepository noteRepository = new NoteRepository();
            Lists list = new Lists();
            list.Projects = noteRepository.GetAllProjects();
            list.Attributes = noteRepository.GetAllAttributes();
            return list;
        }
    }
}