import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  // And what we're going to do is we're going to receive our member from the parent component.
  @Input() member: Member;

  constructor() { }

  ngOnInit(): void {
  }

}
