import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Container, Col, Row } from "reactstrap";
import { Provider } from "react-redux";

import Navigation from "./navigation";
import Dashboard from "./dashboard";
import Toggles from "./toggles";

const Users = () => <h2>Users</h2>;

const AppRouter = ({ store }) => (
  <Provider store={store}>
    <Router>
      <Row>
        <Navigation />
        <main className="col-sm-11" role="main">
          <Route path="/" exact component={Dashboard} />
          <Route path="/toggles/" component={Toggles} />
          <Route path="/users/" component={Users} />
        </main>
      </Row>
    </Router>
  </Provider>
);

export default AppRouter;
