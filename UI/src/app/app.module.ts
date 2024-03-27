import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { NavbarComponent } from './components/navbar/navbar.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { CreateAdvertisementDialogComponent } from './dialogs/create-advertisement-dialog/create-advertisement-dialog.component';
import {MatDialogModule} from '@angular/material/dialog';
import {MatStepperModule} from '@angular/material/stepper';
import {MatFormFieldModule} from '@angular/material/form-field';
import {ReactiveFormsModule} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LoginDialogComponent } from './dialogs/login-dialog/login-dialog.component';
import { RegisterDialogComponent } from './dialogs/register-dialog/register-dialog.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { AdminPanelComponent } from './components/admin/admin-panel/admin-panel.component';
import { SysadminPanelComponent } from './components/sysadmin/sysadmin-panel/sysadmin-panel.component';
import { ModeratorPanelComponent } from './components/moderator/moderator-panel/moderator-panel.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ProfileComponent } from './components/profile/profile/profile.component';
import { AdvertisementComponent } from './components/advertisement/advertisement/advertisement.component';
import { MatGridListModule } from '@angular/material/grid-list';
import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    DashboardComponent,
    CreateAdvertisementDialogComponent,
    LoginDialogComponent,
    RegisterDialogComponent,
    AdminPanelComponent,
    SysadminPanelComponent,
    ModeratorPanelComponent,
    ProfileComponent,
    AdvertisementComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatStepperModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    HttpClientModule,
    ToastrModule.forRoot({
      positionClass: 'toast-top-left'
    }),
    MatProgressSpinnerModule,
    MatGridListModule,
    MatCardModule,
    MatDividerModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
