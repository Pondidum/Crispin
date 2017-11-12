import React, { Component } from "react";
import {
  Button,
  Modal,
  FormGroup,
  ControlLabel,
  FormControl,
  HelpBlock
} from "react-bootstrap";
import { connect } from "react-redux";
import { createToggle } from "./actions";

const mapStateToProps = (state, ownProps) => {
  return state.toggles;
};

const mapDispatchToProps = dispatch => {
  return {
    createToggle: (name, description, success, failure) =>
      dispatch(createToggle(name, description, success, failure))
  };
};

class CreateToggle extends Component {
  constructor() {
    super();

    this.openModal = this.openModal.bind(this);
    this.closeModal = this.closeModal.bind(this);
    this.resetForm = this.resetForm.bind(this);

    this.onNameChange = this.onNameChange.bind(this);
    this.renderForm = this.renderForm.bind(this);

    this.state = {
      showModal: false,
      name: "",
      description: "",
      nameMessages: []
    };
  }

  openModal(e) {
    e.preventDefault();
    this.setState({ showModal: true });
  }

  closeModal() {
    this.resetForm();
    this.setState({ showModal: false });
  }

  resetForm() {
    this.setState({
      name: "",
      description: "",
      nameMessages: []
    });
  }

  onNameChange(name) {
    const inUse = this.props.toggles.find(
      t => t.name.toLowerCase() === name.toLowerCase()
    );

    const nameMessages = [];

    if (inUse)
      nameMessages.push("This name is already in use by another toggle");

    if (!name || name.trim() === "")
      nameMessages.push("The toggle name cannot be blank");

    this.setState({
      name: name,
      nameMessages: nameMessages
    });
  }

  renderForm() {
    return (
      <form>
        <FormGroup
          controlId="toggleName"
          validationState={
            this.state.nameMessages.length === 0 ? "success" : "error"
          }
        >
          <ControlLabel>Name</ControlLabel>
          <FormControl
            type="text"
            placeholder="My-Toggle"
            value={this.state.name}
            onChange={e => this.onNameChange(e.target.value)}
          />
          <HelpBlock>{this.state.nameMessages[0] || ""}</HelpBlock>
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
    );
  }

  render() {
    const save = () =>
      this.props.createToggle(
        this.state.name,
        this.state.description,
        () => this.closeModal(),
        body => this.setState({ nameMessages: body.messages })
      );

    return (
      <span>
        <a href="#" onClick={this.openModal}>
          Create Toggle
        </a>
        <Modal show={this.state.showModal} onHide={this.closeModal}>
          <Modal.Header closeButton>
            <Modal.Title>Create new Toggle</Modal.Title>
          </Modal.Header>
          <Modal.Body>{this.renderForm()}</Modal.Body>
          <Modal.Footer>
            <Button onClick={save} bsStyle="primary">
              Create
            </Button>
            <Button onClick={this.closeModal}>Close</Button>
          </Modal.Footer>
        </Modal>
      </span>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(CreateToggle);
