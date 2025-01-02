import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {from, map, Observable, switchMap} from 'rxjs';
import {LoginDto, LoginResponse, NoteCreate, NoteResponse, RegisterDto} from '../domain/domain';
import {Encryptor} from './security/encryptor';

@Injectable({
  providedIn: 'root'
})
export class BackendService {
  constructor(private http: HttpClient) {}

  login(dto: LoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/auth/login', dto);
  }

  register(dto: RegisterDto): Observable<boolean> {
    return this.http.post<boolean>('/auth/register', dto);
  }

  addNote(noteCreate: NoteCreate): Observable<NoteResponse> {
    // Encrypt the title and content before sending them to the backend
    return from(
      Promise.all([
        Encryptor.encrypt(noteCreate.title),
        Encryptor.encrypt(noteCreate.content),
      ])
    ).pipe(
      switchMap(([encryptedTitle, encryptedContent]) => {
        const encryptedNote = {
          ...noteCreate,
          title: encryptedTitle,
          content: encryptedContent,
        };
        return this.http.post<NoteResponse>('/note', encryptedNote).pipe(
          switchMap((response) =>
            // Decrypt the response data before returning it
            from(
              Promise.all([
                Encryptor.decrypt(response.title), // Decrypt title
                Encryptor.decrypt(response.content), // Decrypt content
              ])
            ).pipe(
              map(([decryptedTitle, decryptedContent]) => ({
                ...response,
                title: decryptedTitle,
                content: decryptedContent,
              }))
            )
          )
        );
      })
    );
  }

  getNoteById(id: number): Observable<NoteResponse> {
    return this.http.get<NoteResponse>(`/note/${id}`).pipe(
      switchMap((response) => {
        return from(
          Encryptor.decrypt(response.content).then(async (decryptedContent) => ({
            ...response,
            content: decryptedContent, // full content decrypted
            title: await Encryptor.decrypt(response.title), // always decrypt title
          }))
        );
      })
    );
  }

  getAllNotes(): Observable<NoteResponse[]> {
    return this.http.get<NoteResponse[]>('/note').pipe(
      switchMap((notes) => {
        // Convert the decryption of each note's title and content to observables
        return from(
          Promise.all(
            notes.map(async (note) => ({
              ...note,
              title: await Encryptor.decrypt(note.title), // Decrypt title
              content: await this.decryptNoteContent(note.content), // Restrict content to first 64 chars
            }))
          )
        );
      })
    );
  }

  private async decryptNoteContent(content: string): Promise<string> {
    const decryptedContent = await Encryptor.decrypt(content);

    let partialContent = decryptedContent.substring(0, 64);
    if (decryptedContent.length > 64) {
      partialContent += '...';
    }

    return partialContent;
  }


  updateNote(noteUpdate: any): Observable<NoteResponse> {
    // Encrypt the title and content before sending them to the backend
    return from(
      Promise.all([
        Encryptor.encrypt(noteUpdate.title),
        Encryptor.encrypt(noteUpdate.content),
      ])
    ).pipe(
      switchMap(([encryptedTitle, encryptedContent]) => {
        const encryptedNote = {
          ...noteUpdate,
          title: encryptedTitle,
          content: encryptedContent,
        };
        return this.http.patch<NoteResponse>('/note', encryptedNote).pipe(
          switchMap((response) =>
            // Decrypt the response data before returning it
            from(
              Promise.all([
                Encryptor.decrypt(response.title), // Decrypt title
                Encryptor.decrypt(response.content), // Decrypt content
              ])
            ).pipe(
              map(([decryptedTitle, decryptedContent]) => ({
                ...response,
                title: decryptedTitle,
                content: decryptedContent,
              }))
            )
          )
        );
      })
    );
  }

  deleteNoteById(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`/note/${id}`);
  }
}
