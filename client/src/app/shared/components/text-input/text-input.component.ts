import { Component, ElementRef, Input, OnInit, Self, ViewChild } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { ignoreElements } from 'rxjs/operators';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
/*
  ControlValueAccessor - Just to highlight what we're doing here this acts as a bridge 
  between a native element and the DOM.
  Now our native element is going to be our input field and we're gonna need to get access 
  to that native element and we can do that by using the Viewchild
 */
export class TextInputComponent implements OnInit, ControlValueAccessor {
  @ViewChild('input') input: ElementRef;
  @Input() type = 'text';
  @Input() label: string;

  /*
  So what we also need to take care of in this particular component is our validation that we're
  also getting from our control value access.
  But in order to access the validation we need to get access to the control itself and the way that
  we can do this is to inject it into our constructor here. And that means we'll be able to access its
  properties and validated inside this component.

  Now what we're going to do for this and we're going to use something called @Self from @angular/core.

  @Self --> this decorator is for angular dependency injection and angular is going to look for where
  to locate what it's going to inject into itself.
  And if we already have a service activated somewhere and our application is going to walk up the tree
  of the Dependency hierarchy looking for something that matches what we're injecting here.
  But if we used the self decorator here it's only going to use this inside itself and not look for any
  other shared dependency that's already in use.
  So this guarantees that we're working with the the very specific control that we're injecting in here.

  NgControl --> is what our form controls derive from.
  public controlDir --> Because this we want to use into out HTML also
   */
  constructor(@Self() public controlDir: NgControl) {
    /*
    And what this does this binds this to our class and now we've got access to our control directive 
    inside our component and we'll have access to it inside our template as well.
     */
    this.controlDir.valueAccessor = this;
  }

  ngOnInit(): void {
    const control = this.controlDir.control;
    const validators = control.validator ? [control.validator] : [];
    const asyncValidtors = control.asyncValidator ? [control.asyncValidator] : [];

    /* we'll pass in our synchronous validators here so the control that we pass from let's say our 
    logging form is going to pass across as validators to this inputs and it's going to set them at the same time
     */
    control.setValidators(validators);
    control.setAsyncValidators(asyncValidtors);
    // this is going to then try and validates our form on initialization
    control.updateValueAndValidity();
  }

  onChange(event): void { }

  onTouched(): void { }

  /*
   for writeValue method, we need to get the value of our inputs and write it into this and
   this gives our ControlValueAccessor access to the values that inputs into our input field.
   */
  writeValue(obj: any): void {
    if (this.input) {
      this.input.nativeElement.value = obj || '';
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  // Not Required
  // setDisabledState?(isDisabled: boolean): void {
  //   throw new Error('Method not implemented.');
  // }
}
