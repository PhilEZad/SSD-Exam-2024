import {AfterViewInit, Component} from '@angular/core';
import {NoteResponse} from '../../domain/domain';
import {CommonModule} from '@angular/common';
import {NgbDropdownModule, NgbModal, NgbModalModule} from '@ng-bootstrap/ng-bootstrap';
import {NoteComponent} from '../note/note.component';
import {ConfirmationModalComponent} from '../confirmation-modal/confirmation-modal.component';
import {AuthService} from '../../services/auth.service';
import {BackendService} from '../../services/backend.service';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule, NgbModalModule],
  templateUrl: './home-page.component.html',
  standalone: true,
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  notes: NoteResponse[] = [];

  constructor(private modal: NgbModal, private authService: AuthService, private backend: BackendService) {
    const userId = this.authService.getTokenData()?.id;

    if (userId) {
      this.backend.getAllNotes().subscribe((notes) => {
        // Only display notes with the same user id
        this.notes = notes.filter(n => n.ownerId === userId);
      });
    }
  }

  onOpen(note: NoteResponse) {
    const ref = this.modal.open(NoteComponent,
      {
        size: 'xl',
        backdrop: 'static',
        keyboard: false
      }
    );

    ref.componentInstance.inputNote(note);


    ref.result.then((result) => {
      if (result) {
        const existingNote = this.notes.find(n => n.id === result.id);

        if (!existingNote) {
          console.error('Note not found');
          return;
        }

        if (result.content === existingNote.content && result.title === existingNote.title) {
          return; // No changes
        } else {
          this.backend.updateNote({
            id: result.id,
            title: result.title,
            content: result.content
          }).subscribe((result) => {
            if (result) {
              const index = this.notes.findIndex(n => n.id === result.id);
              if (index >= 0) {
                this.notes[index] = result;
              }
            }
          });
        }
      }
    });
  }

  deleteNote($event: MouseEvent, note: NoteResponse) {
    $event.stopPropagation();

    const ref = this.modal.open(ConfirmationModalComponent, {size: 'sm'})
    ref.componentInstance.title = 'Delete Note';
    ref.componentInstance.message = 'Delete this note?';

    ref.result.then((result) => {
      if (result) {
        this.backend.deleteNoteById(note.id).subscribe((result) => {
          if (result) {
            this.notes = this.notes.filter(n => n.id !== note.id);
          }
        });
      }
    });
  }

  onNew() {
    const note: NoteResponse = {
      id: 0, title: '', content: '', modified: new Date(), created: new Date(),
      ownerId: -1
    };

    const ref = this.modal.open(NoteComponent,
      {
        size: 'xl',
        backdrop: 'static',
        keyboard: false
      }
    );

    ref.componentInstance.inputNote(note);

    ref.result.then((result) => {
      if (result) {
        const note = result as NoteResponse;

        if (note.content.length === 0) {
          return;
        }

        if (note.title.length === 0) {
          note.title = 'Untitled Note';
        }

        this.backend.addNote({
          title: note.title,
          content: note.content
        }).subscribe((result) => {
          this.notes.push(result);
        });
      }
    });
  }

  logout() {
    this.authService.logout();
  }
}
