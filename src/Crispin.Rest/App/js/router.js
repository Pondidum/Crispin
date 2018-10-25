import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";

import Navigation from "./navigation";
import Dashboard from "./dashboard";

const About = () => <h2>About</h2>;
const Users = () => <h2>Users</h2>;

const AppRouter = () => (
  <Router>
    <div>
      <Navigation />

      <Route path="/" exact component={Dashboard} />
      <Route path="/about/" component={About} />
      <Route path="/users/" component={Users} />
    </div>
  </Router>
);

export default AppRouter;
