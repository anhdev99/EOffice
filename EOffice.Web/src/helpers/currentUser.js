var currentUser = localStorage.getItem('auth-user') ? JSON.parse(localStorage.getItem('auth-user')): {};

const USERNAME = currentUser.userName;
 const FULLNAME = currentUser.fullName;

export const CURRENT_USER = {
    USERNAME, FULLNAME
}