// JavaScript
//try {
//Variables
"use strict";

var HeaderNavBtns = document.getElementsByClassName("Js-navHeader");
var BodyNavBtns = document.getElementsByClassName("page-link");
var HomeBtn = document.getElementById("Home");
//----------------------------------------------

//OnLoad
window.onload = function () {
    //test
    //$("#btny").click(function () {
    //    $("#frm").("https://localhost:44395/Ranking/TopPlayer .ranking-tables", function (date, state) {
    //        $("#t4").text(state);
    //    });
    //});
    //HeaderNavBtns
    {
        //HeaderNavBtns Post
        for (var i = 0; i < HeaderNavBtns.length; i++) {
            HeaderNavBtns[i].addEventListener("click", function (Btn) {
                for (var i = 0; i < HeaderNavBtns.length; i++) {
                    sessionStorage.removeItem(HeaderNavBtns[i].innerHTML + "-Js-navHeader");
                }
                sessionStorage.setItem(Btn.currentTarget.innerHTML + "-Js-navHeader", "active2");
            });
        }
        //HeaderNavBtns Get
        for (var i = 0; i < HeaderNavBtns.length; i++) {
            HeaderNavBtns[i].classList.add(sessionStorage.getItem(HeaderNavBtns[i].innerHTML + "-Js-navHeader"));
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
                    sessionStorage.removeItem(BodyNavBtns[i].innerHTML + "-page-link");
                }
                sessionStorage.setItem(Btn.currentTarget.innerHTML + "-page-link", "active3");
            });
        }
        //BodyNavBtns Get
        for (var i = 0; i < BodyNavBtns.length; i++) {
            BodyNavBtns[i].classList.add(sessionStorage.getItem(BodyNavBtns[i].innerHTML + "-page-link"));
        }
    }
};
//----------------------------------------------
//Functions

//----------------------------------------------

//Server Time
{
    (function () {
        var ServerTime = function ServerTime() {
            var hour = serverTime.innerHTML.split(':')[0];
            var minute = serverTime.innerHTML.split(':')[1];
            var seconds = serverTime.innerHTML.split(':')[2].split(' ')[0];
            seconds++;
            if (seconds == 60) {
                seconds = 0;
                minute++;
            }
            if (minute == 60) {
                minute = 0;
                hour++;
            }
            serverTime.innerHTML = hour.toString().padStart(2, "0") + ":" + minute.toString().padStart(2, "0") + ":" + seconds.toString().padStart(2, "0");
        };

        setInterval(ServerTime, 1000);
        var serverTime = document.getElementById("ServerTimer");
    })();
}
//----------------------------------------------
//Activites
{}

//let ActivitesLables = document.getElementsByClassName("Activites");
//for (var i = 0; i < ActivitesLables.length; i++) {
//    setInterval(Activites, 1000);
//    function Activites() {
//        let hour = ActivitesLables[i].innerHTML.split(':')[0];
//        let minute = ActivitesLables[i].innerHTML.split(':')[1];
//        let seconds = ActivitesLables[i].innerHTML.split(':')[2].split(' ')[0];
//        seconds--;
//        if (seconds <= 0 && minute > 0) {
//            seconds = 60;
//            minute--;
//        }
//        if (minute <= 00 && hour > 0) {
//            minute = 60;
//            hour--;
//        }
//        ActivitesLables[i].innerHTML = `${hour}:${minute}:${seconds}`;
//    }
//}
//----------------------------------------------
//} catch (e) { }

