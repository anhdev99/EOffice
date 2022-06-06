

let eventGuid = 0
// let todayStr = new Date().toISOString().replace(/T.*$/, '') // YYYY-MM-DD of today
var date = new Date();
var d = date.getDate();
var m = date.getMonth();
var y = date.getFullYear();
export const INITIAL_EVENTS = [
    {
        id: 153,
        title: 'Lịch công tác 1',
        start: new Date(y, m, 1),
        className: 'bg-soft-primary',
        location: 'San Francisco, US',
        allDay: false,
        extendedProps: {
            department: 'Trường ĐH Đồng Tháp'
        },
        description: 'Nội dung lịch công tác 1'
    },
    {
        id: 155,
        title: 'Lịch công tác 2',
        start: new Date(y, m, 1),
        className: 'bg-soft-primary',
        location: 'San Francisco, US',
        allDay: false,
        extendedProps: {
            department: 'Trường ĐH Đồng Tháp'
        },
        description: 'Nội dung lịch công tác 2'
    },
    {
        id: 156,
        title: 'Lịch công tác 3',
        start: new Date(y, m, 1),
        className: 'bg-soft-primary',
        location: 'San Francisco, US',
        allDay: false,
        extendedProps: {
            department: 'Trường ĐH Đồng Tháp'
        },
        description: 'Nội dung lịch công tác 3'
    }
]

export function createEventId() {
    return String(eventGuid++)
}

export const categories = [
    {
        name: 'Danger',
        value: 'bg-danger'
    },
    {
        name: 'Success',
        value: 'bg-success'
    },
    {
        name: 'Primary',
        value: 'bg-primary'
    },
    {
        name: 'Info',
        value: 'bg-info'
    },
    {
        name: 'Dark',
        value: 'bg-dark'
    },
    {
        name: 'Warning',
        value: 'bg-warning'
    },
];