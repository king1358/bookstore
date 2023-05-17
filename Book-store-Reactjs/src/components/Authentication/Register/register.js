import React, { useState, useRef } from "react";
import "./style.css";
import axios from "axios";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [confirmP, setConfirmP] = useState("");
  const [fullname, setFullname] = useState("");

  const history = useNavigate();

  //   const cookies = new Cookies();
  const toastId = useRef(null);

  const handleSubmmit = async (e, user) => {
    // console.log("click")
    // console.log(userName)
    // console.log(password)
    e.preventDefault();
    if (password != confirmP) {
      toast("Password confirmation does not match", {
        type: "warning",
        theme: "dark",
        hideProgressBar: false,
        autoClose: 1500,
      });
    } else {
      const form_data = new FormData();
      form_data.set("username", userName);
      form_data.set("password", password);
      form_data.set("fullname", fullname);
      // // console.log(form_data.get("username"));
      // // console.log(form_data.get("password"));
      toastId.current = toast("Processing", {
        theme: "dark",
        type: "info",
        autoClose: 3000,
        hideProgressBar: false,
      });
      await new Promise((resolve) => setTimeout(resolve, 1000));
      axios
        .post(`https://localhost:44348/api/User/Register`, form_data, {
          headers: {
            "Content-Type": "multipart/form-data",
            "Access-Control-Allow-Origin": "*",
          },
        })
        .then((res) => {
          // setProduct(res.data)
          console.log(res.data);
          if (res.data === "Done") {
            //   cookies.set("access_token", res.data.token);
            toast.update(toastId.current, {
              render: "Create account success",
              type: "success",
              theme: "dark",
              hideProgressBar: false,
              autoClose: 1500,
            });
          }
        })
        .catch((error) => {
          console.log("12", error);
          toast.update(toastId.current, {
            render: "Can't login",
            type: "error",
            theme: "dark",
            hideProgressBar: false,
            autoClose: 1500,
          });
        });
    }
  };

  return (
    <div className="Auth-form-container">
      <form
        className="Auth-form"
        onSubmit={handleSubmmit}
        encytpe={"multipart/form-data"}
      >
        <div className="Auth-form-content">
          <h3 className="Auth-form-title">Sign In</h3>
          <div className="form-group mt-3">
            <label>User name</label>
            <input
              type="username"
              className="form-control mt-1"
              placeholder="Enter username"
              onChange={(e) => setUserName(e.target.value)}
              value={userName}
            />
          </div>
          <div className="form-group mt-3">
            <label>Password</label>
            <input
              type="password"
              className="form-control mt-1"
              placeholder="Enter password"
              onChange={(e) => setPassword(e.target.value)}
              value={password}
            />
          </div>
          <div className="form-group mt-3">
            <label>Confirm password</label>
            <input
              type="username"
              className="form-control mt-1"
              placeholder="Enter confirm password"
              onChange={(e) => setConfirmP(e.target.value)}
              value={confirmP}
            />
          </div>
          <div className="form-group mt-3">
            <label>Full name</label>
            <input
              type="username"
              className="form-control mt-1"
              placeholder="Enter fullname"
              onChange={(e) => setFullname(e.target.value)}
              value={fullname}
            />
          </div>
          <div className="d-grid gap-2 mt-3">
            <button
              type="submit"
              className="btn btn-primary"
              //   onClick={() => {

              //   }}
            >
              Submit
            </button>
          </div>
          <p className="forgot-password text-right mt-2">
            Have account? <a href="login">Login</a>
          </p>
        </div>
      </form>
    </div>
  );
}
