import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SysadminPanelComponent } from './components/sysadmin/sysadmin-panel/sysadmin-panel.component';
import { AdminPanelComponent } from './components/admin/admin-panel/admin-panel.component';
import { ModeratorPanelComponent } from './components/moderator/moderator-panel/moderator-panel.component';
import { ProfileComponent } from './components/profile/profile/profile.component';
import { AdvertisementComponent } from './components/advertisement/advertisement/advertisement.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard], data: { requiredPermissions: ['User'] } },
  { path: 'sysadmin', component: SysadminPanelComponent, canActivate: [AuthGuard], data: { requiredPermissions: ['SysAdmin']  } },
  { path: 'admin', component: AdminPanelComponent, canActivate: [AuthGuard], data: { requiredPermissions: ['SysAdmin', 'Admin']  } },
  { path: 'moderator', component: ModeratorPanelComponent, canActivate: [AuthGuard], data: { requiredPermissions: ['SysAdmin', 'Admin', 'Moderator']  } },
  { path: 'advertisements', component: AdvertisementComponent, canActivate: [AuthGuard], data: { requiredPermissions: ['User']  } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
