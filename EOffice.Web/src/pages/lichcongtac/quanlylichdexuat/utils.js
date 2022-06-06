

let eventGuid = 0
// let todayStr = new Date().toISOString().replace(/T.*$/, '') // YYYY-MM-DD of today
var date = new Date();
var d = date.getDate();
var m = date.getMonth();
var y = date.getFullYear();
export const INITIAL_EVENTS = [
    {
        id: 153,
        title: 'Ngày sự kiện',
        start: new Date(y, m, 1),
        className: 'bg-soft-primary',
        location: 'San Francisco, US',
        allDay: false,
        extendedProps: {
            department: 'Ngày sự kiện'
        },
        description: 'Là một ngày co nhiều sự kiện kéo dài'
    },
    {
        id: 136,
        title: 'Công tác Hà nội',
        start: new Date(y, m, d - 5),
        end: new Date(y, m, d - 2),
        allDay: false,
        className: 'bg-soft-warning',
        extendedProps: {
            department: 'Công tác'
        },
        description: 'Đi dạy'
    },
    {
        id: 999,
        title: 'Công tác Đà Nẵng',
        start: new Date(y, m, d + 22, 20, 0),
        end: new Date(y, m, d + 24, 16, 0),
        allDay: false,
        className: 'bg-soft-danger',
        location: 'California, US',
        extendedProps: {
            department: 'Công tác ĐÃ Nẵng'
        },
        description: 'Công tác tại Thành phố Đã Nẵng'
    },
    {
        id: 991,
        title: 'Họp giao ban',
        start: new Date(y, m, d + 4, 16, 0),
        end: new Date(y, m, d + 9, 16, 0),
        allDay: false,
        className: 'bg-soft-primary',
        location: 'Las Vegas, US',
        extendedProps: {
            department: 'Họp giao ban'
        },
        description: 'Họp giao ban ',
    },
    {
        id: 112,
        title: 'Công tác nước ngoài',
        start: new Date(y, m, d, 12, 30),
        allDay: false,
        className: 'bg-soft-success',
        location: 'Head Office, US',
        extendedProps: {
            department: 'Công tác nước ngoài'
        },
        description: 'Triển khai hợp tác quốc tế '
    },
    {
        id: 113,
        title: 'Weekly Strategy Planning',
        start: new Date(y, m, d + 9),
        end: new Date(y, m, d + 11),
        allDay: false,
        className: 'bg-soft-danger',
        location: 'Head Office, US',
        extendedProps: {
            department: 'Lunch'
        },
        description: 'Strategies for Creating Your Weekly Schedule'
    },
    {
        id: 875,
        title: 'Triển khai công tác đôi ngoại',
        start: new Date(y, m, d + 1, 19, 0),
        allDay: false,
        className: 'bg-soft-success',
        location: 'Los Angeles, US',
        extendedProps: {
            department: 'Công tác'
        },
        description: 'Triển khai công tác đối ngoại, đẩy nhanh tiến độ hợp tác quốc tế'
    },
    {
        id: 456,
        title: 'Công tác',
        start: new Date(y, m, d + 23, 20, 0),
        end: new Date(y, m, d + 24, 16, 0),
        allDay: false,
        className: 'bg-soft-info',
        location: 'Head Office, US',
        extendedProps: {
            department: 'Công tác'
        },
        description: 'Công tác nước ngoài'
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