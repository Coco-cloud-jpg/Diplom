import axios from "axios";

var refreshRequest = false;

export const post = async (url, payload) => {
    const access = await refreshTokens();

    const config = {
        headers:{
            Authorization: `Bearer ${access}`
        }
      };

    return await axios.post(url, payload, config);
 }

 export const get = async (url) => {
    const access = await refreshTokens();

    const config = {
        headers:{
            Authorization: `Bearer ${access}`
        }
      };

    return await axios.get(url, config);
 }

 export const put = async (url, payload) => {
    const access = await refreshTokens();

    const config = {
        headers:{
            Authorization: `Bearer ${access}`
        }
      };

    return await axios.put(url, payload, config);
 }

 export const destroy = async (url) => {
    const access = await refreshTokens();

    const config = {
        headers:{
            Authorization: `Bearer ${access}`
        }
      };

    return await axios.delete(url, config);
 }


 export const patch = async (url) => {
  const access = await refreshTokens();

  const config = {
      headers:{
          Authorization: `Bearer ${access}`
      }
    };

  return await axios.patch(url, {}, config);
}


 export const refreshTokens = async () => {
    let {access, refresh, validUntil} = JSON.parse(localStorage.getItem("tokens"));

    if (validUntil < Date.now() && !refreshRequest) {
        refreshRequest = true;
        var response = await axios.post(`https://localhost:7063/api/token/refresh/${refresh}`);
        console.log(response);
        access = response.data.access;
        localStorage.setItem("tokens", JSON.stringify(response.data));
        refreshRequest = false;
    }

    return access;
 }