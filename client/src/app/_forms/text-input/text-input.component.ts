import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  // And what we want to do here is we want to implement a control value assessor.

  @Input() label: string;
  @Input() type = 'text';

  // But what Angular will do when it's looking at dependency injection,
  // it's going to look inside the hierarchy of things that it can inject.
  // If there's an injector that matches this that it's already got inside its dependency injection container,
  // then it's going to attempt to reuse that one. We don't want that to happen for this.
  // We want our text input components to be self contained, and we don't want Angular to try and fetch us another instance of what we're doing here.
  // We always want this to be self contained, and this decorator ensures that Angular will always inject
  // what we're doing here locally into this component.
  constructor(@Self() public ngControl: NgControl) {
    // And by adding this, now we've got access to our control inside this component when we use it inside our register form.
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {

  }

  registerOnChange(fn: any): void {

  }

  registerOnTouched(fn: any): void {

  }

}
