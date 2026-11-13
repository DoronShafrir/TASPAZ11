function updateUserInterface() {

    if (IsAdmin === 'Admin') {
        const adminElements = document.querySelectorAll(".admin-only");

        adminElements.forEach(element => {
            element.style.display = 'block';
        });
    }
    else if (IsTeacher === 'Teacher') {
        const teacherElements = document.querySelectorAll(".teacher-only");

        teacherElements.forEach(element => {
            element.style.display = 'block';
        });
    }
    else if (IsUser !== '') {
        const adminElements1 = document.querySelectorAll(".admin-only");

        adminElements1.forEach(element => {
            element.style.display = 'None';
        });

        const userElements = document.querySelectorAll(".user-only");

        userElements.forEach(element => {
            element.style.display = 'None';
        });
    }
}