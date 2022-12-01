import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {MatDividerModule} from "@angular/material/divider";
import { SideNavAdminComponent } from './side-nav-admin/side-nav-admin.component';
import { AppRoutingModule } from './app-routing.module';
import { SideNavUserComponent } from './side-nav-user/side-nav-user.component';
import { DeviceOverviewComponent } from './userPages/device-overview/device-overview.component';
import { DeviceRegistrationComponent } from './userPages/device-registration/device-registration.component';
import { ProfileInfoComponent } from './userPages/profile-info/profile-info.component';
import { ContactInfoComponent } from './userPages/contact-info/contact-info.component';
import { SupportComponent } from './userPages/support/support.component';
import { NotFoundComponent } from './userPages/not-found/not-found.component';
import { AdminLoginOverviewComponent } from './AdminPages/admin-login-overview/admin-login-overview.component';
import { LoginComponent } from './login/login.component';
import {MatInputModule} from "@angular/material/input";
import {FormsModule} from "@angular/forms";
import { RegisterComponent } from './register/register.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import {AdminDeviceOverviewComponent} from "./AdminPages/admin-device-overview/admin-device-overview.component";
import {
  AdminDeviceRegistrationComponent
} from "./AdminPages/admin-device-registration/admin-device-registration.component";
import {AdminSupportComponent} from "./AdminPages/admin-support/admin-support.component";
import {UserOverviewComponent} from "./AdminPages/user-overview/user-overview.component";
import {MatTableModule} from "@angular/material/table";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatSortModule} from "@angular/material/sort";
import {MatCardModule} from "@angular/material/card";


// @ts-ignore
@NgModule({
  declarations: [
    AppComponent,
    SideNavAdminComponent,
    SideNavUserComponent,
    DeviceOverviewComponent,
    DeviceRegistrationComponent,
    ProfileInfoComponent,
    ContactInfoComponent,
    SupportComponent,
    NotFoundComponent,
    AdminLoginOverviewComponent,
    LoginComponent,
    RegisterComponent,
    LoadingSpinnerComponent,
    AdminDeviceOverviewComponent,
    AdminDeviceRegistrationComponent,
    AdminSupportComponent,
    UserOverviewComponent
  ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatToolbarModule,
        MatSidenavModule,
        MatButtonModule,
        MatIconModule,
        MatDividerModule,
        AppRoutingModule,
        MatInputModule,
        FormsModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatCardModule
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
