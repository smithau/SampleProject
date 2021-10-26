import { Component, OnInit } from '@angular/core';
import { IAttribute } from 'src/models/i-attribute';
import { INote } from 'src/models/i-note';
import { IProject } from 'src/models/i-project';
import { NoteService } from 'src/services/note/note.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {
  filterProjectId: number = 0;
  filterAttributeIds: number[] = [];
  projectNoteCounts: any[] = [];
  attributeNoteCounts: any[] = [];
  projects: IProject[] = []
  attributes: IAttribute[] = []
  notes: INote[] = [];

  constructor(
    private noteService: NoteService
  ) { }

  ngOnInit(): void {
    this.filterNotes();
    this.noteService.getLists().subscribe(lists => {
      this.projects = lists.Projects;
      this.attributes = lists.Attributes;
    })
  }

  filterNotes(){
    this.noteService.getNotes(this.filterAttributeIds, this.filterProjectId).subscribe(notes => {
      console.log(notes)
      this.notes = notes;
      this.updateCounts();
    })
  }

  addNote(){
    let newNote: INote = {
      NoteText: ""
    }
    this.noteService.newNote(newNote).subscribe(results => {
      this.notes.push(newNote);
      this.updateCounts();
    })
  }

  updateNote(note: INote){
    if(note.NoteId){
      this.noteService.updateNote(note.NoteId, note.NoteText).subscribe(result => {
        this.updateCounts();
      })
    }
  }

  deleteNote(note: INote){
    if(note.NoteId){
      this.noteService.deleteNote(note.NoteId).subscribe(result => {
        this.notes = this.notes.filter(innerNote => innerNote.NoteId !== note.NoteId);
        this.updateCounts();
      })
    }
  }

  updateCounts(){
    this.noteService.getProjectNoteCount().subscribe(projectCounts => {
      this.projectNoteCounts = projectCounts;
    })
    this.noteService.getAttributeNoteCount().subscribe(attributeCounts => {
      this.attributeNoteCounts = attributeCounts;
    })
  }
}
