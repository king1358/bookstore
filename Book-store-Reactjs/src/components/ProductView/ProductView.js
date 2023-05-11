import React from 'react'
import { Container, Grid, Button, Typography } from '@material-ui/core';
import { Link } from 'react-router-dom';
import { commerce } from '../../lib/commerce';
import { useState, useEffect } from "react";
import './style.css'
import axios from "axios";

const createMarkup = (text) => {
  return { __html: text };
};

const ProductView = () => {

  const [product, setProduct] = useState({});
  const [temp, setTemp] = useState({});

  const fetchProduct = async (id) => {
    axios.get(`http://localhost:30766/api/Book/id?id=${id}`, {
      headers: {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
      }
    })
      .then(res => {
        setProduct(res.data)
        console.log(res.data)
      })
      .catch(error => console.log(error));
    // console.log({ response });
    // const { name, price, media, quantity, description } = response;
  };

  useEffect(() => {
    const id = window.location.pathname.split("/");
    fetchProduct(id[2]);
  }, []);

  return (
    <Container className="product-view">
      <Grid container>
        <Grid item xs={12} md={6} className="image-wrapper">
          <img src={product.sourceimg} alt={product.nameb}
          />
        </Grid>
        <Grid item xs={12} md={5} className="text">
          <Typography variant="h2"><b>{product.nameb}</b></Typography>
          <hr />
          <Typography variant="p" dangerouslySetInnerHTML={createMarkup(product.descr)} />
          <Typography variant="h3" color="secondary" >Price: <b> {product.price} </b> </Typography>
          <br />
          <Grid container spacing={4}>
            <Grid item xs={12}>
              <Button size="large" className="custom-button" component={Link} to='/' >
                Continue Shopping
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    </Container>
  );
};

export default ProductView;
