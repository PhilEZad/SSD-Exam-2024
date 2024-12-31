import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {LoginDto, LoginResponse, NoteCreate, NoteResponse, RegisterDto} from '../domain/domain';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor(private http: HttpClient) { }

  login(dto: LoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('auth/login', dto);
  }

  register(dto: RegisterDto): Observable<boolean> {
    return this.http.post<boolean>('auth/register', dto);
  }

  addNote(noteCreate: NoteCreate): Observable<NoteResponse> {
    return this.http.post<NoteResponse>('/note', noteCreate);
  }

  getNoteById(id: number): Observable<NoteResponse> {
    return this.http.get<NoteResponse>(`/note/${id}`);
  }

  getAllNotes(): Observable<NoteResponse[]> {
    return this.http.get<NoteResponse[]>('/note');
  }

  updateNote(noteUpdate: any): Observable<any> {
    return this.http.patch<NoteResponse>(`/note`, noteUpdate);
  }

  deleteNoteById(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`/note/${id}`);
  }
}
