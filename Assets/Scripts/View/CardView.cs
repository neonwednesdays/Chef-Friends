﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour, BaseView {
    public CardElement Element;

    // properties
    private RectTransform rectTransform;
    public Vector3 Position {
        get { return rectTransform.anchoredPosition; }
    }

    // view timing
    private int timer = 0; // animation time remaining

    // animation data
    private string _animationName; // name of the animation being used
    public string animationName {
        get { return _animationName; }
        set {
            _animationName = value;
            this.handleAnimationUpdate(value);
        }
    }
    public string animationType; // name of the animation being used
    public int animationTime; // length of animation
    public Vector3 Origin; // animation beginning position
    public List<Vector3> PathPoints;
    public Vector3 Destination { // animation end position
        get { return Element.Model.Position; }
    }
    public bool destroyAfterAnimation; // destroy this card GameObject after animating

    // data
    private Text textComponent;
    private string _Text;
    public string Text {
        get { return _Text; }
        set {
            _Text = value;
            textComponent.text = value;
        }
    }
    // public Vector3 handRelativePosition; // this card's position in the hand

    void Awake() {
        GameObject CardTextObject = transform.Find("UI_Text").gameObject;
        textComponent = CardTextObject.GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start() {

        // transform.localScale = Vector3.zero;
    }
    
    void Update() {
        if (timer > 0) {
            float animPercent = 1.0f - ((float)timer / (float)animationTime);

            // -- type
            if (animationType == AnimationConstants.QUADRATIC_ANIM_TYPE) {
                rectTransform.anchoredPosition = CurveHelper.getQuadraticBezier(Origin, PathPoints[0], Destination, animPercent);
            }
            if (animationType == AnimationConstants.LINEAR_ANIM_TYPE) {
                rectTransform.anchoredPosition = CurveHelper.getQuadraticBezier(Origin, PathPoints[0], Destination, animPercent);
            }

            // -- card specific
            if (animationName == CardConstants.DRAW_CARD_ANIM) {    
                transform.localScale = Vector3.one * animPercent;
            }

            // done animating a frame
            timer --;

            if (timer == 0) {
                animationType = null;
                this.handleViewDoneAnimation();

                if (destroyAfterAnimation) {
                    this.handleDestroy();
                }
            }
        }
    }

    // -- animations
    public void handleAnimationUpdate(string animName) {
        if (this.isAnimatable()) {
            if (animName == CardConstants.DRAW_CARD_ANIM) {
                this.handleDrawCardAnimation();

            } else if (animName == CardConstants.USE_CARD_ANIM) {
                this.handleUseCardAnimation();

            } else if (animName == CardConstants.MOVE_CARD_ANIM) {
                this.setAnimationDefaultData(CardConstants.MOVE_CARD_ANIM, CardConstants.MOVE_CARD_ANIM_TIME);
                animationType = AnimationConstants.LINEAR_ANIM_TYPE;
            }
            
            // Debug.Log("handleAnimationUpdate data " + Origin + " ... " + PathPoints[0] + " ... " + Destination);
        }
    }
    /* called when Model's position has changed */
    public void handlePositionUpdate() {
        this.handleAnimationUpdate(CardConstants.MOVE_CARD_ANIM);
    }
    /* animate when the card was drawn */
    public void handleDrawCardAnimation() {
        this.setAnimationDefaultData(CardConstants.DRAW_CARD_ANIM, CardConstants.DRAW_CARD_ANIM_TIME);
        animationType = AnimationConstants.LINEAR_ANIM_TYPE;
    }

    /* animate when the card was used */
    public void handleUseCardAnimation() {
        this.setAnimationDefaultData(CardConstants.USE_CARD_ANIM, CardConstants.USE_CARD_ANIM_TIME);
        destroyAfterAnimation = true;
        animationType = AnimationConstants.QUADRATIC_ANIM_TYPE;

        // Destination = new Vector3(Position.x + 1, Position.y - 1.5f, Position.z);

        Vector3 midpoint = (Destination + Origin) / 2;
        PathPoints = new List<Vector3> { midpoint };
    }

    // -- helpers
    /* handles setting a basic set up for animation data */
    private void setAnimationDefaultData(string newAnimName, int newAnimTime) {
        // reset some stuff
        destroyAfterAnimation = false;

        Vector3 midpoint = (Destination + Origin) / 2;
        PathPoints = new List<Vector3> { midpoint };

        // set
        Origin = Position;

        // animationName = newAnimName;
        animationTime = newAnimTime;
        timer = newAnimTime;
    }
    public bool isInteractable() {
        return timer == 0;
    }
    public bool isAnimatable() {
        return timer == 0 && (animationType == null || animationType == "");
    }

    // -- MonoBehavior
    void OnMouseUp() {
        if (this.isInteractable()) {
            Element.Controller.handleOnMouseUp();
        }
    }

    void OnMouseOver() {

    }

    void OnMouseExit() {

    }

    // -- interface implementation
    public void handleViewDoneAnimation() {
        Element.Controller.handleOnDoneAnimation();
    }
    public void handleDestroy() {
        Element.Controller.handleViewBeforeDestroy();
        Destroy(gameObject);
    }

    /* trying to keep certain variables private */
    public void setElement(CardElement e) {
        Element = e;
    }
    public void setDefaultPosition(Vector3 pos) {
        Origin = pos;
        rectTransform.anchoredPosition = pos;
    }

}
