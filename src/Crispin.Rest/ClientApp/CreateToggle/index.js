import React, { Component } from "react";
import { Button, Modal } from "react-bootstrap";
import { FormGroup, ControlLabel, FormControl } from "react-bootstrap";
import { connect } from "react-redux";
import { createToggle } from "./actions";

const mapDispatchToProps = dispatch => {
  return {
    createToggle: (name, description, closeForm) =>
      dispatch(createToggle(name, description, closeForm))
  };
};

class CreateToggle extends Component {
  constructor() {
    super();
    this.state = {
      showModal: false,
      name: "",
      description: ""
    };
  }

  render() {
    const open = () => this.setState({ showModal: true });
    const close = () => this.setState({ showModal: false });

    const save = () => {
      this.props.createToggle(this.state.name, this.state.description, close);
    };

    const toggleModal = e => {
      e.preventDefault();
      open();
    };

    return (
      <span>
        <a href="#" onClick={toggleModal}>
          Create Toggle
        </a>
        <Modal show={this.state.showModal} onHide={close}>
          <Modal.Header closeButton>
            <Modal.Title>Create new Toggle</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <form>
              <FormGroup controlId="toggleName">
                <ControlLabel>Name</ControlLabel>
                <FormControl
                  type="text"
                  placeholder="My-Toggle"
                  value={this.state.name}
                  onChange={e => this.setState({ name: e.target.value })}
                />
              </FormGroup>
              <FormGroup controlId="toggleDescription">
                <ControlLabel>Description</ControlLabel>
                <FormControl
                  type="text"
                  placeholder="some short description of the toggle"
                  value={this.state.description}
                  onChange={e => this.setState({ description: e.target.value })}
                />
              </FormGroup>
            </form>
          </Modal.Body>
          <Modal.Footer>
            <Button onClick={save} bsStyle="primary">
              Create
            </Button>
            <Button onClick={close}>Close</Button>
          </Modal.Footer>
        </Modal>
      </span>
    );
  }
}

export default connect(null, mapDispatchToProps)(CreateToggle);
