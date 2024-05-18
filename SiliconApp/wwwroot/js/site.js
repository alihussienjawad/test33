var toastMixin = Swal.mixin({
    toast: true,
    icon: 'success',
    title: 'General Title',
    animation: false,
    position: 'top-right',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
});

$(function () {
  
 
    GetUserInfo();
    if (getPreferredTheme() === 'dark') {
        $('#dark-light-mode').attr('checked',true)
    }
    else {
        $('#dark-light-mode').attr('checked', false)
    }
    $('#Input_DeleteAccount').on("change", (e) => {

        const { checked } = e.target;
        $('#delete_account_btn').toggleClass("disabled-link")
      
    })
    $('#termsCheckbox').on("change", (e) => {
        $('#registerSubmit').toggleClass("disabled")
    })

    $('#imgupload').on("change", (e) => {
        var tmppath = URL.createObjectURL(e.target.files[0]);
        Swal.fire({
            title: "Change User Image!",
            text: "Do You Want To Use This Image As Your Profile Iamge?",
            icon: "warning",
            imageUrl: tmppath,
            imageWidth: 400,
            imageHeight: 200,
            imageAlt: "Person image",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, Use it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $('#per-img-form').trigger("submit");
                Swal.fire({
                    title: "Upload!",
                    text: "Your Profile Image Was Updated.",
                    icon: "success"
                });
            }
        });
       
    })

   
});
const deleteMyAccount = () => {
    Swal.fire({
        title: "Delete Your Account!",
        text: "Do You Want To Delete Your Account?",
        icon: "danger",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $('#delete-account-form').submit();
            Swal.fire({
                title: "Upload!",
                text: "Your Account Deleted.",
                icon: "success"
            });
        }
    });
}
const LoadPersonImg = () => {
    $('#imgupload').trigger('click');
}

$(window).on('resize', () => {
    let zoom = Number((window.innerWidth / window.screen.width).toFixed(3));
   // document.firstElementChild.style.zoom = zoom;
})
$(window).on('scroll', function () {
    if ($(this).scrollTop() > 50) {
        $("#header").stop(true).fadeTo(400, 0.9);
    } else {
        $("#header").stop(true).fadeTo(400, 1);
    }
});

const GetUserInfo = () => {
    $.get(`/home/GetUserInfo`)
        .then(res => {
            $('#user-img').attr("src", res.userImg)
        })
}
const DarkLight = (value) => {
    if (value) {
        localStorage.setItem('theme', "dark") 
    }
    else {
        localStorage.setItem('theme', "light")
        
    }
    setTheme(getPreferredTheme())
    
}
const UploadImage = (input) => {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#courseImg').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
const SaveCourse = (courseId,IsSaved) => {
    $.get(`/Courses/SaveCourse?CourseId=${courseId}&IsSaved=${IsSaved}`)
        .then(res => {
            if (res.status) {
                toastMixin.fire({
                    animation: true,
                    title:  res.message
                });
            } else {
                toastMixin.fire({
                    animation: true,
                    icon:'error',
                    title: res.message
                });
            }
        }).then(() => {
            let category = $('#CourseCategory option:selected').val();
            let search = $('#SearchCourse').val();
            $.get(`/Courses/_indexCourseUser?CourseCategory=${category}&,SearchCourse=${search}`)
                .then(res => {
                    $('#course-content').html(res);
                })
        })
}

// To access the stars
let stars =
    document.getElementsByClassName("star");
let output =
    document.getElementById("output");

// Funtion to update rating
function gfg(n) {
    remove();
    for (let i = 0; i < n; i++) {
        if (n == 1) cls = "one";
        else if (n == 2) cls = "two";
        else if (n == 3) cls = "three";
        else if (n == 4) cls = "four";
        else if (n == 5) cls = "five";
        stars[i].className = "star " + cls;
    }
    output.innerText = "Rating is: " + n + "/5";
    $('.rating').val(n)
}

// To remove the pre-applied styling
function remove() {
    let i = 0;
    while (i < 5) {
        stars[i].className = "star";
        i++;
        
    }
  
}
const GetSavedCourses=() =>{
    $.get(`/Courses/GetSavedCourseUser`)
        .then(res => {
            $('#savedcourses-div').html(res)

        })
}
const DeleteAll = () => {
    $.get(`/Courses/DeleteAllSavedItem`)
        .then(res => {
            $('#savedcourses-div').html(res)
        })

}
const RemoveSaveCourse = (id) => {
    $.get(`/Courses/RemoveSaveCourse?id=${id}`)
        .then(res => {
            $('#savedcourses-div').html(res)
        })
}