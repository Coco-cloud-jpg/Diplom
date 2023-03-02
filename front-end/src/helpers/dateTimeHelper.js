export const getDateTimeString = (date) => {
    return `${date.toLocaleTimeString()} ${date.toLocaleDateString()}`
}

export const secondsToTime = (d) => {
    d = Number(d);
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);
    
    return `${pad(h)}:${pad(m)}:${pad(s)}`; 
}

function pad(d) {
    return (d < 10) ? '0' + d.toString() : d.toString();
}