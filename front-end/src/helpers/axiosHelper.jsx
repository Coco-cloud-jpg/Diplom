import axios from "axios";

export const post = async (url, payload) => {
    let {access, refresh, validUntil} = JSON.parse(localStorage.getItem("tokens"));

    if (validUntil < Date.now()) {
        var response = await axios.post(`https://localhost:7063/api/token/refresh/${refresh}`);
        access = response.access;
        localStorage.setItem("tokens", JSON.stringify(response.data))
    }

    const config = {
        headers:{
            Authorization: `Bearer ${access}`
        }
      };

    return await axios.post(url, payload, config);
 }