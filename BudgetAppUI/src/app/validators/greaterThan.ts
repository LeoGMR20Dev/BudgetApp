import { AbstractControl, ValidationErrors } from '@angular/forms';

export function greaterThan(value: number) {
  return (control: AbstractControl): ValidationErrors | null => {
    if (control.value === null) return null;

    return control.value > value
      ? null
      : { greaterThan: { requiredValue: value } };
  };
}
