import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRolPermissionComponent } from './form-rol-permission.component';

describe('FormRolPermissionComponent', () => {
  let component: FormRolPermissionComponent;
  let fixture: ComponentFixture<FormRolPermissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormRolPermissionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormRolPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
