import React, { useState, useEffect } from 'react';
import { CssBaseline } from '@material-ui/core';
import { commerce } from './lib/commerce';
import Products from './components/Products/Products';
import Navbar from './components/Navbar/Navbar';
import Cart from './components/Cart/Cart';
import Checkout from './components/CheckoutForm/Checkout/Checkout';
import ProductView from './components/ProductView/ProductView';
import Footer from './components/Footer/Footer';
import Login from './components/Authentication/Login/Login';

import jwt_decode from "jwt-decode";
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'mdbreact/dist/css/mdb.css';
import '@fortawesome/fontawesome-free/css/all.min.css';
import axios from "axios";
import Cookies from 'universal-cookie';

const App = () => {
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState([]);
  const [order, setOrder] = useState({});
  const [errorMessage, setErrorMessage] = useState('');
  const [book, setBook] = useState([]);

  const getApiData = async () => {
    axios.get(`http://localhost:30766/api/Book`, {
      headers: {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
      }
    })
      .then(res => {
        setBook(res.data)
        console.log(res.data)
      })
      .catch(error => console.log(error));
  };

  // const fetchProducts = async () => {
  //   const { data } = await commerce.products.list();

  //   setProducts(data);
  // };

  // const fetchCart = async () => {
  //   setCart(await commerce.cart.retrieve());
  // };
  const cookies = new Cookies();
  const handleAddToCart = (productId, quantity) => {
    const token = cookies.get('access_token')
    let decoded = jwt_decode(token);
    let datasend = {
      "id_user": decoded.username,
      "id_book": productId,
      "quantity": quantity
    }
    // console.log(datasend)
    axios.post('http://localhost:30766/api/Cart', datasend, {
      headers: {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
      },
    }).then((res)=>{
      console.log(res)
    })
  };

  const handleUpdateCartQty = async (lineItemId, quantity) => {
      handleAddToCart(lineItemId,quantity)
  };

  const handleRemoveFromCart = async (lineItemId) => {
    const response = await commerce.cart.remove(lineItemId);

    setCart(response.cart);
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
      const incomingOrder = await commerce.checkout.capture(checkoutTokenId, newOrder);

      setOrder(incomingOrder);

      refreshCart();
    } catch (error) {
      setErrorMessage(error.data.error.message);
    }
  };

  useEffect(() => {
    getApiData();
    // fetchProducts();
    // fetchCart();
  }, []);

  const handleDrawerToggle = () => setMobileOpen(!mobileOpen);

  return (
    <div>
      <Router>
        <div style={{ display: 'flex' }}>
          <CssBaseline />
          <Navbar totalItems={cart.total_items} handleDrawerToggle={handleDrawerToggle} />
          <Switch>
            <Route exact path="/">
              <Products products={book} onAddToCart={handleAddToCart} />
            </Route>
            <Route exact path="/cart/:username">
              <Cart onUpdateCartQty={handleUpdateCartQty} onRemoveFromCart={handleRemoveFromCart} onEmptyCart={handleEmptyCart} />
            </Route>
            <Route path="/checkout" exact>
              <Checkout cart={cart} order={order} onCaptureCheckout={handleCaptureCheckout} error={errorMessage} />
            </Route>
            <Route path="/product-view/:id" exact>
              <ProductView />
            </Route>
            <Route path="/login" exact>
              <Login />
            </Route>
          </Switch>
        </div>
      </Router>
      <Footer />
    </div>
  );
};

export default App;
