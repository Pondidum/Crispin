import React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';

const routes = <Layout>
<Route exact path='/' component={ Home } />
</Layout>

module.exports = routes
