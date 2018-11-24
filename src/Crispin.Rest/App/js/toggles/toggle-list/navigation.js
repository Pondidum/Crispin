import React, { Component } from "react";
import { Col, Nav } from "reactstrap";

import Header from "./header";
import Filter from "./filter";
import MenuEntry from "./menu-entry";

import "./toggle-list.css";

class Navigation extends Component {
  constructor(props) {
    super(props);
    this.state = { filter: "" };
  }

  render() {
    const filter = this.state.filter;
    const where = this.props.where;

    const items = filter
      ? this.props.items.filter(item => where(item, filter))
      : this.props.items;

    return (
      <Col sm="3" md="2" className="sidebar">
        <Header
          updating={this.props.updating}
          buttons={this.props.headerButtons}
        />
        {this.props.children}
        <Filter onFilterChanged={value => this.setState({ filter: value })} />
        <Nav vertical className="sidebar-sticky">
          {items.map(t => (
            <MenuEntry key={t.id} match={this.props.match} toggle={t} />
          ))}
        </Nav>
      </Col>
    );
  }
}

export default Navigation;
