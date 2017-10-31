import React from 'react';
import { Route } from 'react-router-dom';
import Layout from './Layout'
import Dashboard from './Dashboard'

const routes = <Layout>
  <Route exact path='/' component={ Dashboard } />
</Layout>

module.exports = routes
