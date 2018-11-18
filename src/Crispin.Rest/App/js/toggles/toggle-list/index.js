import { connect } from "react-redux";
import React, { Component } from "react";
import { Col, Nav } from "reactstrap";

import Header from "./header";
import Filter from "./filter";
import CreateToggleDialog from "../create";
import MenuEntry from "./menu-entry";

import "./toggle-list.css";

import { fetchAllToggles } from "../actions";

const mapPropsFromState = (state, ownProps) => {
  return {
    ...ownProps,
    updating: state.toggles.updating,
    toggles: state.toggles.all
  };
};

const mapDispatchToProps = dispatch => {
  return {
    refresh: () => dispatch(fetchAllToggles())
  };
};

const connector = connect(
  mapPropsFromState,
  mapDispatchToProps
);

class Navigation extends Component {
  constructor(props) {
    super(props);
    this.state = { filter: "" };

    this.createDialog = React.createRef();
  }

  render() {
    const handleRefresh = e => {
      e.preventDefault();
      this.props.refresh();
    };

    const filter = this.state.filter;
    const filteredToggles = filter
      ? this.props.toggles.filter(t => t.name.toLowerCase().includes(filter))
      : this.props.toggles;

    return (
      <Col sm="3" md="2" className="sidebar">
        <Header
          updating={this.props.updating}
          handleCreate={() => this.createDialog.current.toggle()}
          handleRefresh={handleRefresh}
        />
        <CreateToggleDialog ref={this.createDialog} />
        <Filter onFilterChanged={value => this.setState({ filter: value })} />
        <Nav vertical className="sidebar-sticky">
          {filteredToggles.map(t => (
            <MenuEntry key={t.id} match={this.props.match} toggle={t} />
          ))}
        </Nav>
      </Col>
    );
  }
}

export default connector(Navigation);
