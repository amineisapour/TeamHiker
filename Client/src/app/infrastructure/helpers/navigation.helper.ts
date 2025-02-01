import { Injectable } from '@angular/core';
import * as Models from 'src/app/models/menu-item.model';
import * as Interfaces from 'src/app/interfaces/menu-item.interface';

//NavigationItem => Interfaces
//Navigation => Models

// const NavigationItems = [
//     new Models.MenuItem(1, 'Dashboard', 'item', '', 'admin/dashboard', true, null),
//     new Models.MenuItem(2, 'Tasks', 'collapse', '', '', true, [
//         new Models.MenuItem(3, 'Tasks Board', 'item', '', 'admin/tasks/board', false, null),
//         new Models.MenuItem(4, 'Tasks Departments', 'collapse', '', '', false, [
//             new Models.MenuItem(5, 'Tasks DV', 'item', '', 'admin/tasks/departments/dv', false, null),
//             new Models.MenuItem(6, 'Tasks Admin', 'item', '', 'admin/tasks/departments/admin', false, null),
//             new Models.MenuItem(7, 'Tasks FC', 'item', '', 'admin/tasks/departments/fc', false, null),
//         ]),
//         new Models.MenuItem(8, 'Tasks Report', 'collapse', '', '', false, [
//             new Models.MenuItem(9, 'Report 1', 'item', '', 'admin/tasks/report/1', false, null),
//             new Models.MenuItem(10, 'Report 2', 'item', '', 'admin/tasks/report/2', false, null),
//             new Models.MenuItem(11, 'Report 3', 'item', '', 'admin/tasks/report/3', false, null),
//         ]),
//     ]),
//     new Models.MenuItem(12, 'User', 'collapse', '', '', false, [
//         new Models.MenuItem(13, 'Profile', 'item', '', 'admin/user/profile', false, null),
//         new Models.MenuItem(14, 'User List', 'item', '', 'admin/user/list', false, null),
//         new Models.MenuItem(15, 'User Access', 'item', '', 'admin/user/access', false, null),
//     ]),
// ];

const NavigationItems = [
    new Models.MenuItem(1, 'Dashboard', 'item', '', 'admin/dashboard', true, null),
    new Models.MenuItem(2, 'Operational Efficiency', 'collapse', '', '', true, [
        new Models.MenuItem(3, 'Contact Management', 'item', '', 'admin/tasks/board1', false, null),
        new Models.MenuItem(4, 'Tours Planning', 'item', '', 'admin/tasks/board2', false, null),
        new Models.MenuItem(5, 'Community', 'item', '', 'admin/tasks/board3', false, null),
    ]),
    new Models.MenuItem(6, 'Revenue Growth', 'collapse', '', '', true, [
        new Models.MenuItem(7, 'Promotions', 'item', '', 'admin/tasks/board4', false, null),
        new Models.MenuItem(8, 'Affiliate', 'item', '', 'admin/tasks/board5', false, null),
        new Models.MenuItem(9, 'Equipment Rent', 'item', '', 'admin/tasks/board6', false, null),
        new Models.MenuItem(10, 'Online Courses', 'item', '', 'admin/tasks/board7', false, null),
    ]),
    new Models.MenuItem(11, 'Safety & Control', 'collapse', '', '', true, [
        new Models.MenuItem(12, 'Reports', 'item', '', 'admin/tasks/board8', false, null),
        new Models.MenuItem(13, 'Live Tracking', 'item', '', 'admin/tasks/board9', false, null),
        new Models.MenuItem(14, 'SOS Message', 'item', '', 'admin/tasks/board10', false, null),
    ]),
    new Models.MenuItem(15, 'Account', 'collapse', '', '', false, [
        new Models.MenuItem(16, 'Profile', 'item', '', 'admin/user/profile', false, null),
        new Models.MenuItem(17, 'User List', 'item', '', 'admin/user/list', false, null),
    ]),
];

@Injectable()
export class NavigationHelper {
    public get(): Interfaces.MenuItem[] {
        return NavigationItems;
    }
}
