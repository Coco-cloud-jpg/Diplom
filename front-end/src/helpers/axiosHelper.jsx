import axios from "axios";

var refreshRequest = false;

export const downloadFile = async (url) => { 
  const access = await refreshTokens();

  const config = {
      headers:{
          Authorization: `Bearer ${access}`
      },
      responseType: 'blob' 
    };

  const response = await axios.get(url, config);
  let filename = 'filename.pdf';
  const contentDisposition = response.headers['content-disposition'];
  if (contentDisposition) {
    const match = contentDisposition.match(/filename="(.+)"/);
    if (match && match[1]) {
      filename = match[1];
    }
  }

  const urlObject = window.URL.createObjectURL(new Blob([response.data]));

  const tempLink = document.createElement('a');
  tempLink.href = urlObject;
  tempLink.setAttribute('download', filename); 
  document.body.appendChild(tempLink);
  tempLink.click();

  window.URL.revokeObjectURL(urlObject);
  document.body.removeChild(tempLink);
};

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