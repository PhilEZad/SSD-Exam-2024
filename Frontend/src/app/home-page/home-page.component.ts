import {AfterViewInit, Component} from '@angular/core';
import {NoteResponse} from '../../domain/domain';
import {CommonModule} from '@angular/common';
import {NgbDropdownModule, NgbModal, NgbModalModule} from '@ng-bootstrap/ng-bootstrap';
import {NoteComponent} from '../note/note.component';
import {ConfirmationModalComponent} from '../confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule, NgbModalModule],
  templateUrl: './home-page.component.html',
  standalone: true,
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent  {
  notes: NoteResponse[] = [
    {id: 1, title: 'Note 1', content: 'This is the content of note 1.', created: new Date(), modified: new Date()},
    {
      id: 2,
      title: 'Note 2',
      content: 'This is some longer content of note 2.',
      created: new Date(),
      modified: new Date()
    },
    {id: 3, title: 'Note 3', content: 'Content for note 3.', created: new Date(), modified: new Date()},
    {
      id: 4,
      title: 'Note 4',
      content: 'Here is some additional content for note 4.',
      created: new Date(),
      modified: new Date()
    },
    {id: 5, title: 'Note 5', content: 'Note 5 has more details here.', created: new Date(), modified: new Date()},
    {id: 6, title: 'Note 6', content: 'Content of note 6, shorter.', created: new Date(), modified: new Date()},
  ];

  constructor(private modal: NgbModal) { }


  onOpen(note: NoteResponse) {
    const ref = this.modal.open(NoteComponent,
      {
        size: 'xl'
      }
    );

    ref.componentInstance.inputNote(note);


    ref.result.then((result) => {
      if (result) {
        console.log('Save note', result);
        const index = this.notes.findIndex(n => n.id === result.id);
        if (index >= 0) {
          this.notes[index] = result;
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
        console.log('Delete note', note);
        this.notes = this.notes.filter(n => n.id !== note.id);
      }
    });
  }

  onNew() {
    const note: NoteResponse = {id: 0, title: '', content: '', modified: new Date(), created: new Date()};

    const ref = this.modal.open(NoteComponent,
      {
        size: 'xl'
      }
    );

    ref.componentInstance.inputNote(note);

    ref.result.then((result) => {
      if (result) {
        console.log('Save note', result);
        this.notes.push(result);
      }
    });
  }
}
