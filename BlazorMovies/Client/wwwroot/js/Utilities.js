function my_function(message) {
    console.log("From Utilities" + message);
}


function dotnetStaticInvocation() {
    DotNet.invokeMethodAsync("BlazorMovies.Client", "GetCurrentCount")
        .then(result => {
            console.log("Count from Javascript " + result);
        });
}

function dotnetInstanceInvocation(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}

function initializeInactivityTimer(dotnetHelper) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimout(timer);
        timer = setTimeout(logout, 3000);
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}