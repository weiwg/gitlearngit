import jwtDecode from "jwt-decode";

export const GetTokeUserInfo = () => {
  return jwtDecode(localStorage.getItem("token"));
}