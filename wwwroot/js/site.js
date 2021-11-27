// JavaScript

//Variables
let HeaderNavBtns = document.getElementsByClassName("Js-navHeader");
let BodyNavBtns = document.getElementsByClassName("page-link");
let HomeBtn = document.getElementById("Home");
let Activites = document.getElementsByClassName("Activites");
let Lb_serverTime = document.getElementById("ServerTimer");
//----------------------------------------------

window.onload = function () {
    //HeaderNavBtns
    {
        //HeaderNavBtns Post
        for (var i = 0; i < HeaderNavBtns.length; i++) {
            HeaderNavBtns[i].addEventListener("click", function (Btn) {
                for (var i = 0; i < HeaderNavBtns.length; i++) {
                    sessionStorage.removeItem(`${HeaderNavBtns[i].innerHTML}-Js-navHeader`);
                }
                sessionStorage.setItem(`${Btn.currentTarget.innerHTML}-Js-navHeader`, "active2");
            })
        }
        //HeaderNavBtns Get
        for (var i = 0; i < HeaderNavBtns.length; i++) {
            HeaderNavBtns[i].classList.add(
                sessionStorage.getItem(`${HeaderNavBtns[i].innerHTML}-Js-navHeader`));
        }

        //Check First open
        if (sessionStorage.length == 0) {
            HomeBtn.classList.add("active2");
            if (BodyNavBtns[1].innerHTML == "1") {
                BodyNavBtns[1].classList.add("active3");
            }
        }
    }

    //BodyNavBtns
    {
        //BodyNavBtns Post
        for (var i = 0; i < BodyNavBtns.length; i++) {
            BodyNavBtns[i].addEventListener("click", function (Btn) {
                for (var i = 0; i < BodyNavBtns.length; i++) {
                    sessionStorage.removeItem(`${BodyNavBtns[i].innerHTML}-page-link`);
                }
                sessionStorage.setItem(`${Btn.currentTarget.innerHTML}-page-link`, "active3");
            })
        }
        //BodyNavBtns Get
        for (var i = 0; i < BodyNavBtns.length; i++) {
            BodyNavBtns[i].classList.add(
                sessionStorage.getItem(`${BodyNavBtns[i].innerHTML}-page-link`));
        }
    }

    //Server Time
    {
        let hours = Lb_serverTime.innerHTML.split(':')[0];
        let minutes = Lb_serverTime.innerHTML.split(':')[1];
        let seconds = Lb_serverTime.innerHTML.split(':')[2].split(' ')[0];
        setInterval(ServerTime, 1000);
        function ServerTime() {
            if (hours == 23 && minutes == 59 && seconds == 59) {
                hours = 0;
                minutes = 0;
                seconds = 0;
            }
            seconds++;
            if (seconds == 60) {
                seconds = 0;
                minutes++;
            }
            if (minutes == 60) {
                minutes = 0;
                hours++;
            }
            Lb_serverTime.innerHTML =
                `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
        }
    }

    //Activites
    {
        for (let i = 0; i < Activites.length; i++) {
            setInterval(function () {
                if (!Activites[i].innerHTML.includes("d")) {
                    let hour = Activites[i].innerHTML.split(':')[0];
                    let min = Activites[i].innerHTML.split(':')[1];
                    let sec = Activites[i].innerHTML.split(':')[2];

                    sec--;
                    if (sec <= 0 && min > 0) {
                        sec = 60;
                        min--;
                    }
                    if (min <= 0 && hour > 0) {
                        min = 59;
                        hour--;
                    }
                    if (min == 0 && hour == 0 && sec == 0) {
                        hour = 23;
                        min = 59;
                        sec = 60;
                    }
                    Activites[i].innerHTML =
                        `${hour.toString().padStart(2, "0")}:${min.toString().padStart(2, "0")}:${sec.toString().padStart(2, "0")}`;
                }
                else {
                    let day = Activites[i].innerHTML.split(',')[0].split('d')[0];
                    let hour = Activites[i].innerHTML.split(',')[1].split(':')[0];
                    let min = Activites[i].innerHTML.split(',')[1].split(':')[1];
                    let sec = Activites[i].innerHTML.split(',')[1].split(':')[2];
                    console.log(day);
                    console.log(hour);
                    console.log(min);
                    console.log(sec);

                    sec--;
                    if (sec <= 0 && min > 0) {
                        sec = 60;
                        min--;
                    }
                    if (min <= 0 && hour > 0) {
                        min = 59;
                        hour--;
                    }
                    if (min == 0 && hour == 0 && sec == 0 && day > 0) {
                        hour = 23;
                        min = 59;
                        sec = 60;
                        day--;
                    }
                    Activites[i].innerHTML =
                        `${day}d, ${hour.toString().padStart(2, "0")}:${min.toString().padStart(2, "0")}:${sec.toString().padStart(2, "0")}`;
                }
            }, 1000);
        }
    }
}