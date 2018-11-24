import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Row } from "reactstrap";
import { Provider } from "react-redux";

import Navigation from "./navigation";
import Dashboard from "./dashboard";
import Toggles from "./toggles";

const Stats = () => <h2>Stats</h2>;
const Users = () => <h2>Users</h2>;

const AppRouter = ({ store }) => (
  <Provider store={store}>
    <Router>
      <Row>
        <div className="col col-md-05 sidebar">
          <div className="sidebar-sticky bg-darker">
            <Navigation />
          </div>
        </div>
        <main className="col col-md-115" role="main">
          <Route path="/" exact component={Dashboard} />
          <Route path="/toggles/" component={Toggles} />
          <Route path="/stats/" component={Stats} />
          <Route path="/users/" component={Users} />
        </main>
      </Row>
    </Router>
  </Provider>
);

export default AppRouter;
