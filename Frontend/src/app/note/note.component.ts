import {AfterViewInit, Component} from '@angular/core';
import {NoteResponse} from '../../domain/domain';
import {NgbActiveModal, NgbModalModule} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {DatePipe} from '@angular/common';
import {BackendService} from '../../services/backend.service';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-note',
  imports: [NgbModalModule, FormsModule, DatePipe],
  templateUrl: './note.component.html',
  standalone: true,
  styleUrl: './note.component.scss'
})
export class NoteComponent implements AfterViewInit{
  note: NoteResponse = {
    id: 0,
    title: 'Untitled Note',
    content: '',
    created: new Date(),
    modified: new Date(),
    ownerId: -1
  }

  private readonly _userId: number = -1;

  constructor(private active: NgbActiveModal, private backend: BackendService, private auth: AuthService) {
    this._userId = this.auth.getTokenData()?.id ?? -1;
  }

  ngAfterViewInit(): void {
    // find textArea and set focus
    const textArea = document.getElementById('content') as HTMLTextAreaElement;
    textArea.focus();
  }

  public inputNote(note: NoteResponse) {
    if (this._userId === -1) {
      console.error('User not logged in');
      return;
    }

    if (note.id <= 0) {
      this.note = {...note};
    }
    else {
      this.backend.getNoteById(note.id).subscribe((note) => {
        if (note.ownerId !== this._userId) {
          console.error('Note does not belong to user');
          this.active.dismiss();
        }
        this.note = note;
      });
    }
  }


  save() {
    this.active.close(this.note);
  }
}
