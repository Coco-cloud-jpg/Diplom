export const getDateTimeString = (date) => {
    return `${date.toLocaleTimeString()} ${date.toLocaleDateString()}`
}

export const secondsToTime = (d) => {
    d = Number(d);
    let h = Math.floor(d / 3600);
    let m = Math.floor(d % 3600 / 60);
    let s = Math.floor(d % 3600 % 60);
    
    return `${pad(h)}:${pad(m)}:${pad(s)}`; 
}

function pad(d) {
    return (d < 10) ? '0' + d.toString() : d.toString();
}