export const getDateTimeString = (date) => {
    return `${date.toLocaleTimeString()} ${date.toLocaleDateString()}`
}