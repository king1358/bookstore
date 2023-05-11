import React, { useState } from "react"
import './style.css'
import axios from "axios";
import jwt_decode from "jwt-decode";
import { useCookies } from 'react-cookie'
import Cookies from 'universal-cookie';

export default function Login(props) {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const cookies = new Cookies();

    const handleSubmmit = (e, user) => {

        // console.log("click")
        // console.log(userName)
        // console.log(password)
        e.preventDefault()
        
        const form_data = new FormData()
        form_data.set("username", userName)
        form_data.set("password", password)

        console.log(form_data.get("username"))
        console.log(form_data.get("password"))

        axios.post(`http://localhost:30766/login`, form_data, {
            headers: {
                // "Content-Type": "multipart/form-data",
                "Access-Control-Allow-Origin": "*"
            },
        })
            .then(res => {
                // setProduct(res.data)
                console.log(res.data)
                if (res.data.result === "Success"){
                    cookies.set('access_token', res.data.token);
                }
            })
            .catch(error => console.log(error));
    }

    return (
        <div className="Auth-form-container">
            <form className="Auth-form" onSubmit={handleSubmmit} encytpe={"multipart/form-data"}>
                <div className="Auth-form-content">
                    <h3 className="Auth-form-title">Sign In</h3>
                    <div className="form-group mt-3">
                        <label>User name</label>
                        <input
                            type="username"
                            className="form-control mt-1"
                            placeholder="Enter username"
                            onChange={e => setUserName(e.target.value)}
                        />
                    </div>
                    <div className="form-group mt-3">
                        <label>Password</label>
                        <input
                            type="password"
                            className="form-control mt-1"
                            placeholder="Enter password"
                            onChange={e => setPassword(e.target.value)}
                        />
                    </div>
                    <div className="d-grid gap-2 mt-3">
                        <button type="submit" className="btn btn-primary"
                        // // onClick={() => {
                        // //     console.log(userName);
                        // //     console.log(password);
                        // // }
                        // }
                        >
                            Submit
                        </button>
                    </div>
                    <p className="forgot-password text-right mt-2">
                        Forgot <a href="#">password?</a>
                    </p>
                </div>
            </form>
        </div>
    )
}