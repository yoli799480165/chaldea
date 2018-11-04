import { Component, OnInit } from '@angular/core';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTES: RouteInfo[] = [
    { path: 'dashboard', title: 'Dashboard', icon: 'pe-7s-home', class: '' },
    { path: 'bangumi', title: '番组管理', icon: 'pe-7s-science', class: '' },
    { path: 'anime', title: '动漫管理', icon: 'pe-7s-display1', class: '' },
    { path: 'node-resource', title: '资源管理', icon: 'pe-7s-video', class: '' },
    { path: 'timetable', title: '更新时间表', icon: 'pe-7s-date', class: '' },
    { path: 'icons', title: '消息管理', icon: 'pe-7s-chat', class: '' },
    { path: 'icons', title: '评论管理', icon: 'pe-7s-paper-plane', class: '' },
    { path: 'icons', title: '成就管理', icon: 'pe-7s-medal', class: '' },
    { path: 'icons', title: '用户管理', icon: 'pe-7s-users', class: '' },
    { path: 'maps', title: '系统设置', icon: 'pe-7s-settings', class: '' }
];

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html'
})
export class SidebarComponent implements OnInit {
    menuItems: any[];

    constructor() { }

    ngOnInit() {
        this.menuItems = ROUTES.filter(menuItem => menuItem);
    }
    isMobileMenu() {
        if ($(window).width() > 991) {
            return false;
        }
        return true;
    };
}
