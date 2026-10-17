function checkInsertValidity() {
    let newCourseName = document.getElementById("newCourseName").value;
    let newCourseNumber = document.getElementById("newCourseNumber").value;
    let newCourseTeacher = document.getElementById("newCourseTeacher").value;
    if (newCourseName.trim() ==="") { alert("Course Name is empty"); return false; }  
    else if(newCourseNumber.trim() ==="") { alert("Course Number is empty"); return false; }
    else if(newCourseTeacher.trim() ==="") { alert("Course Teacher is empty"); return false; }
         
    return true;    
}
