import { connect } from "react-redux";
import React, { Component } from "react";
import { Col, Nav, Input } from "reactstrap";
import MenuEntry from "./menu-entry";
import Updating from "./updating";
import Glyph from "../../util/glyph";
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
        <div className="toggle-list-header">
          <nav className="navbar navbar-expand navbar-dark bg-dark justify-content-between ">
            <div className="navbar-nav">
              <a className="nav-item nav-link" href="#" onClick={handleRefresh}>
                <Glyph name="plus" alt="Create new Toggle" />
              </a>
            </div>
            <div className="navbar-nav">
              <a className="nav-item nav-link" href="#" onClick={handleRefresh}>
                <Glyph name="sync" alt="Refresh toggles" />
              </a>
            </div>
          </nav>
          <Updating updating={this.props.updating} />
        </div>
        <Input
          bsSize="sm"
          placeholder="filter..."
          onChange={e =>
            this.setState({ filter: e.target.value.toLowerCase() })
          }
        />
        <Nav vertical className="sidebar-sticky">
          {filteredToggles.map(t => (
            <MenuEntry
              key={t.id}
              match={this.props.match}
              id={t.id}
              name={t.name}
            />
          ))}
        </Nav>
      </Col>
    );
  }
}
// const Navigation = ({ match, toggles, updating, refresh }) => {
//   let filter; // = React.createRef();

//   const filteredToggles = toggles.filter(t => {
//     console.log(filter);
//     return true;
//   });

//   const onChange = e => {
//     filter = e.target.value.toLowerCase();
//   };
// };

export default connector(Navigation);
