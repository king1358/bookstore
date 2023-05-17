import axios from "axios";
import React, { useState, useEffect } from "react";
import jwt_decode from "jwt-decode";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const CheckoutTemp = () => {
  const history = useNavigate();

  useEffect(() => {
    let token = sessionStorage.getItem("access_token");
    if (token) {
      let decode = jwt_decode(token);
      let data = {
        id_u: decode.id,
        token: token,
      };
      console.log("data", data);
      axios
        .post("https://localhost:44348/api/Cart/Checkout", data, {
          headers: {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
          },
        })
        .then((res) => {
          if (res.data == "Done") {
            toast("Checkout success", {
              type: "success",
              autoClose: 1500,
            });
            history("/");
          } else {
            toast("Checkout error", {
              type: "error",
              autoClose: 1500,
            });
            history("/cart");
          }
        });
    }
  }, []);
  return <div></div>;
};

export default CheckoutTemp;
