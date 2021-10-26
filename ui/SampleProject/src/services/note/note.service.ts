import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { INote } from 'src/models/i-note';
import { HttpClient } from '@angular/common/http';
import { IProject } from 'src/models/i-project';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  url = `http://localhost:60977/api/note`;

  constructor(private http: HttpClient) { }

  updateNote(noteId: number, text: string): Observable<string> {
    return this.http.get<string>(this.url + `/update-note?noteId=${noteId}&noteText=${text}`)
  }

  newNote(newNoteEntry: INote): Observable<string> {
    return this.http.post<string>(this.url + `/create-note`, newNoteEntry);
  }

  deleteNote(noteId: number): Observable<string> {
    return this.http.post<string>(this.url + `/delete-note?noteId=${noteId}`, null);
  }

  newProject(project: IProject): Observable<string> {
    return this.http.post<string>(this.url + `/create-project`, project);
  }

  getNotes(attributeIds?: number[], projectId?: number): Observable<INote[]> {
    if (projectId)
      return this.http.post<INote[]>(this.url + `/get-notes?projectId=${projectId}`, attributeIds);
    else
      return this.http.post<INote[]>(this.url + `/get-notes`, attributeIds);

  }

  getProjectNoteCount(): Observable<any[]> {
    return this.http.get<any[]>(this.url + `/get-project-note-counts`);
  }

  getAttributeNoteCount(): Observable<any[]> {
    return this.http.get<any[]>(this.url + `/get-attribute-note-counts`);
  }

  getLists(): Observable<any> {
    return this.http.get<any>(this.url + `/get-lists`);
  }
}
