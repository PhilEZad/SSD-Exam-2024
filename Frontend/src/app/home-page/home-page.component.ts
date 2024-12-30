import {AfterViewInit, Component} from '@angular/core';
import {NoteResponse} from '../../domain/domain';
import {CommonModule} from '@angular/common';
import {NgbDropdownModule} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule, NgbDropdownModule],
  templateUrl: './home-page.component.html',
  standalone: true,
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements AfterViewInit {
  notes: NoteResponse[] = [
    { id: 1, title: 'Note 1', content: 'This is the content of note 1.' },
    { id: 2, title: 'Note 2', content: 'This is some longer content of note 2.' },
    { id: 3, title: 'Note 3', content: 'Content for note 3.' },
    { id: 4, title: 'Note 4', content: 'Here is some additional content for note 4.' },
    { id: 5, title: 'Note 5', content: 'Note 5 has more details here.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },
    { id: 6, title: 'Note 6', content: 'Content of note 6, shorter.' },

  ];

  ngAfterViewInit(): void {

  }
}
