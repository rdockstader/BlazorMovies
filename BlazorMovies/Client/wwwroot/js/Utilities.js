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

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 1000 * 60 * 20); // 20 minutes
    }

    

    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;
}

