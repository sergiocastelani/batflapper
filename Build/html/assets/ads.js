
function PrepareAd()
{}

function ShowAd()
{
    document.getElementById("adOverlay").style.display = "block";

    //close button timer
    let closeButton = document.getElementById("closeIcon");
    let closeTimerText = document.getElementById("closeTimerText");
    closeButton.style.display = "none";
    closeTimerText.style.display = "block";
    closeTimerText.innerText = "5 s";
    let initialTime = (new Date()).getTime();
    let closeIntervalId = setInterval(() => 
    {
        const now = (new Date()).getTime();
        const timeLeft = 5000 - (now - initialTime);

        if (timeLeft <= 0)
        {
            closeButton.style.display = "block";
            closeTimerText.style.display = "none";
            clearInterval(closeIntervalId);
        }
        else
        {
            closeTimerText.innerText = `${(Math.ceil(timeLeft / 1000)).toFixed(0)} s`;
        }
    }, 300);
}

function closeAd()
{
    document.getElementById("adOverlay").style.display = "none";
}