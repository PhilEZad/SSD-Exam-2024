import {AfterViewInit, Component} from '@angular/core';
import {NoteResponse} from '../../domain/domain';
import {NgbActiveModal, NgbModalModule} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {DatePipe} from '@angular/common';

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
    title: '',
    content: '',
    created: new Date(),
    modified: new Date(),
    ownerId: -1
  }



  constructor(private active: NgbActiveModal) {

  }

  ngAfterViewInit(): void {
    // find textArea and set focus
    const textArea = document.getElementById('content') as HTMLTextAreaElement;
    textArea.focus();
  }

  public inputNote(note: NoteResponse) {
    this.note = {...note};
  }


  save() {
    this.active.close(this.note);
  }
}
