import React, { useState, useEffect } from "react";
import { CssBaseline } from "@material-ui/core";
import { commerce } from "./lib/commerce";
import Products from "./components/Products/Products";
import Navbar from "./components/Navbar/Navbar";
import Cart from "./components/Cart/Cart";
import Checkout from "./components/CheckoutForm/Checkout/Checkout";
import ProductView from "./components/ProductView/ProductView";
import Footer from "./components/Footer/Footer";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "mdbreact/dist/css/mdb.css";
import "@fortawesome/fontawesome-free/css/all.min.css";
import axios from "axios";
import Login from "./components/Authentication/Login/login";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import jwt_decode from "jwt-decode";
import { QueryClient, QueryClientProvider } from "react-query";
import Register from "./components/Authentication/Register/register";
import CheckoutTemp from "./components/CheckoutForm/checkoutTemp";
const App = () => {
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState({});
  const [order, setOrder] = useState({});
  const [errorMessage, setErrorMessage] = useState("");
  const [token, setToken] = useState("");
  // const fetchProducts = async () => {
  //   axios
  //     .get(`https://localhost:44348/api/Book`)
  //     .then((res) => {
  //       setProducts(res.data);
  //     })
  //     .catch((error) => console.log(error));
  // };

  // const fetchCart = async (token) => {
  //   let decode = jwt_decode(token);
  //   axios
  //     .get(`https://localhost:44348/api/Cart?id=${decode.id}`)
  //     .then((res) => {
  //       console.log("CART", res.data);
  //       setCart(res.data);
  //     })
  //     .catch((error) => console.log(error));
  //   // setCart(await commerce.cart.retrieve());
  // };

  const loginToken = (result) => {
    console.log(result);
    setToken(result);
  };

  const handleEmptyCart = async () => {
    const response = await commerce.cart.empty();

    setCart(response.cart);
  };

  const refreshCart = async () => {
    const newCart = await commerce.cart.refresh();

    setCart(newCart);
  };

  const handleCaptureCheckout = async (checkoutTokenId, newOrder) => {
    try {
      const incomingOrder = await commerce.checkout.capture(
        checkoutTokenId,
        newOrder
      );

      setOrder(incomingOrder);

      refreshCart();
    } catch (error) {
      setErrorMessage(error.data.error.message);
    }
  };

  useEffect(() => {
    // fetchProducts();
    // fetchCart();
    // if (token) alert("AAAAA");
  }, []);

  const handleDrawerToggle = () => setMobileOpen(!mobileOpen);
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <div>
        <Router>
          <div style={{ display: "flex" }}>
            <CssBaseline />
            <Navbar
              totalItems={cart.total_items}
              handleDrawerToggle={handleDrawerToggle}
            />
            <Routes>
              <Route
                exact
                path="/"
                element={<Products handleUpdateCartQty />}
              />
              <Route
                exact
                path="/cart"
                element={<Cart onEmptyCart={handleEmptyCart} />}
              />
              <Route
                path="/checkout"
                exact
                element={
                  <Checkout
                    cart={cart}
                    order={order}
                    onCaptureCheckout={handleCaptureCheckout}
                    error={errorMessage}
                  />
                }
              />
              <Route path="/product-view/:id" exact element={<ProductView />} />

              <Route
                path="/login"
                exact
                element={<Login loginToken={loginToken} />}
              />
              <Route path="/register" exact element={<Register />} />
              <Route path="/checkoutTemp" exact element={<CheckoutTemp />} />
            </Routes>
            <ToastContainer />
          </div>
        </Router>
        <Footer />
      </div>
    </QueryClientProvider>
  );
};

export default App;
